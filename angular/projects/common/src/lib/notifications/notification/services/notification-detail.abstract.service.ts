import { inject, Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ListService } from '@abp/ng.core';
import { finalize, tap } from 'rxjs/operators';
import type { NotificationDto } from '../../../proxy/notifications/models';
import { NotificationService } from '../../../proxy/notifications/notification.service';

export abstract class AbstractNotificationDetailViewService {
  protected readonly fb = inject(FormBuilder);
  public readonly proxyService = inject(NotificationService);
  public readonly list = inject(ListService);

  isBusy = false;
  isVisible = false;
  selected = {} as any;
  form: FormGroup | undefined;

  buildForm() {
    const { fromUserId, toUserId, notiTitle, notiBody, isRead, docId, url, type } =
      this.selected || {};

    this.form = this.fb.group({
      fromUserId: [fromUserId ?? null, [Validators.required]],
      toUserId: [toUserId ?? null, []],
      notiTitle: [notiTitle ?? null, [Validators.required]],
      notiBody: [notiBody ?? null, []],
      isRead: [isRead ?? false, [Validators.required]],
      docId: [docId ?? null, [Validators.required]],
      url: [url ?? null, [Validators.required]],
      type: [type ?? null, []],
    });
  }

  showForm() {
    this.buildForm();
    this.isVisible = true;
  }

  create() {
    this.selected = undefined;
    this.showForm();
  }

  update(record: NotificationDto) {
    this.selected = record;
    this.showForm();
  }

  hideForm() {
    this.isVisible = false;
    this.form.reset();
  }

  submitForm() {
    if (this.form.invalid) return;

    this.isBusy = true;

    const request = this.createRequest().pipe(
      finalize(() => (this.isBusy = false)),
      tap(() => this.hideForm())
    );

    request.subscribe(this.list.get);
  }

  private createRequest() {
    if (this.selected) {
      return this.proxyService.update(this.selected.id, {
        ...this.form.value,
        concurrencyStamp: this.selected.concurrencyStamp,
      });
    }
    return this.proxyService.create(this.form.value);
  }

  changeVisible($event: boolean) {
    this.isVisible = $event;
  }
}
