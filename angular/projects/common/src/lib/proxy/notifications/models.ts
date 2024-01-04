import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';
import type { NotificationsType } from './notifications-type.enum';

export interface GetNotificationsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  fromUserId?: string;
  toUserId?: string;
  notiTitle?: string;
  notiBody?: string;
  isRead?: boolean;
  docId?: string;
  url?: string;
  type?: NotificationsType;
}

export interface NotificationCreateDto {
  fromUserId: string;
  toUserId?: string;
  notiTitle: string;
  notiBody?: string;
  isRead: boolean;
  docId: string;
  url: string;
  type?: NotificationsType;
}

export interface NotificationDto extends AuditedEntityDto<string> {
  fromUserId: string;
  toUserId?: string;
  notiTitle: string;
  notiBody?: string;
  isRead: boolean;
  docId: string;
  url: string;
  type?: NotificationsType;
  concurrencyStamp?: string;
}

export interface NotificationUpdateDto {
  fromUserId: string;
  toUserId?: string;
  notiTitle: string;
  notiBody?: string;
  isRead: boolean;
  docId: string;
  url: string;
  type?: NotificationsType;
  concurrencyStamp?: string;
}
