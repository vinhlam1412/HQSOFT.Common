import { ListService, TrackByService } from '@abp/ng.core';
import { Component, OnInit, inject } from '@angular/core';

import { notificationsTypeOptions } from '../../../proxy/notifications/notifications-type.enum';
import type { NotificationDto } from '../../../proxy/notifications/models';
import { NotificationViewService } from '../services/notification.service';
import { NotificationDetailViewService } from '../services/notification-detail.service';

@Component({
  template: '',
})
export abstract class AbstractNotificationComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(NotificationViewService);
  public readonly serviceDetail = inject(NotificationDetailViewService);
  protected title = 'Common::Notifications';

  notificationsTypeOptions = notificationsTypeOptions;

  ngOnInit() {
    this.service.hookToQuery();
  }

  clearFilters() {
    this.service.clearFilters();
  }

  showForm() {
    this.serviceDetail.showForm();
  }

  create() {
    this.serviceDetail.selected = undefined;
    this.serviceDetail.showForm();
  }

  update(record: NotificationDto) {
    this.serviceDetail.update(record);
  }

  delete(record: NotificationDto) {
    this.service.delete(record);
  }
}
