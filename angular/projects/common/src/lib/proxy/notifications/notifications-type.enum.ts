import { mapEnumToOptions } from '@abp/ng.core';

export enum NotificationsType {
  Mention = 0,
  Assignment = 1,
  Alert = 2,
  Share = 3,
}

export const notificationsTypeOptions = mapEnumToOptions(NotificationsType);
