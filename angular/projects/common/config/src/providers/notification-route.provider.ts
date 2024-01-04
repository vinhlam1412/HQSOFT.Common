import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';
import { eCommonRouteNames } from '../enums/route-names';

export const NOTIFICATIONS_NOTIFICATION_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: '/common/notifications',
        parentName: eCommonRouteNames.Common,
        name: 'Common::Menu:Notifications',
        layout: eLayoutType.application,
        requiredPolicy: 'Common.Notifications',
      },
    ]);
  };
}
