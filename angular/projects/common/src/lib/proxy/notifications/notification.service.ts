import type {
  GetNotificationsInput,
  NotificationCreateDto,
  NotificationDto,
  NotificationUpdateDto,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  apiName = 'Common';

  create = (input: NotificationCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, NotificationDto>(
      {
        method: 'POST',
        url: '/api/common/notifications',
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/common/notifications/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, NotificationDto>(
      {
        method: 'GET',
        url: `/api/common/notifications/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  getList = (input: GetNotificationsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<NotificationDto>>(
      {
        method: 'GET',
        url: '/api/common/notifications',
        params: {
          filterText: input.filterText,
          fromUserId: input.fromUserId,
          toUserId: input.toUserId,
          notiTitle: input.notiTitle,
          notiBody: input.notiBody,
          isRead: input.isRead,
          docId: input.docId,
          url: input.url,
          type: input.type,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config }
    );

  update = (id: string, input: NotificationUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, NotificationDto>(
      {
        method: 'PUT',
        url: `/api/common/notifications/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  constructor(private restService: RestService) {}
}
