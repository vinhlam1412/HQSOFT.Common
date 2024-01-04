import { NgModule } from '@angular/core';
import { NotificationComponent } from './components/notification.component';
import { NotificationRoutingModule } from './notification-routing.module';

@NgModule({
  declarations: [],
  imports: [NotificationComponent, NotificationRoutingModule],
})
export class NotificationModule {}

export function loadNotificationModuleAsChild() {
  return Promise.resolve(NotificationModule);
}
