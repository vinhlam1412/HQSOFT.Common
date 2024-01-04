import { Injectable, inject } from '@angular/core';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { ABP, ListService, PagedResultDto } from '@abp/ng.core';
import { filter, switchMap } from 'rxjs/operators';
import type { GetNotificationsInput, NotificationDto } from '../../../proxy/notifications/models';
import { NotificationService } from '../../../proxy/notifications/notification.service';

export abstract class AbstractNotificationViewService {
  protected readonly proxyService = inject(NotificationService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);

  data: PagedResultDto<NotificationDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {} as GetNotificationsInput;

  delete(record: NotificationDto) {
    this.confirmationService
      .warn('Common::DeleteConfirmationMessage', 'Common::AreYouSure', {
        messageLocalizationParams: [],
      })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id))
      )
      .subscribe(this.list.get);
  }

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
        filterText: query.filter,
      });

    const setData = (list: PagedResultDto<NotificationDto>) => (this.data = list);

    this.list.hookToQuery(getData).subscribe(setData);
  }

  clearFilters() {
    this.filters = {} as GetNotificationsInput;
    this.list.get();
  }
}
