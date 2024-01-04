import { ModuleWithProviders, NgModule } from '@angular/core';
import { COMMON_ROUTE_PROVIDERS } from './providers/route.provider';
import { COMMENTS_COMMENT_ROUTE_PROVIDER } from './providers/comment-route.provider';
import { NOTIFICATIONS_NOTIFICATION_ROUTE_PROVIDER } from './providers/notification-route.provider';

@NgModule()
export class CommonConfigModule {
  static forRoot(): ModuleWithProviders<CommonConfigModule> {
    return {
      ngModule: CommonConfigModule,
      providers: [
        COMMON_ROUTE_PROVIDERS,
        COMMENTS_COMMENT_ROUTE_PROVIDER,
        NOTIFICATIONS_NOTIFICATION_ROUTE_PROVIDER,
      ],
    };
  }
}
