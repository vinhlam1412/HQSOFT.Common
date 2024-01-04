import { ListService, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { DateAdapter } from '@abp/ng.theme.shared/extensions';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import {
  NgbDateAdapter,
  NgbCollapseModule,
  NgbDatepickerModule,
  NgbDropdownModule,
} from '@ng-bootstrap/ng-bootstrap';
import { NgxValidateCoreModule } from '@ngx-validate/core';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { PageModule } from '@abp/ng.components/page';

import { notificationsTypeOptions } from '../../../proxy/notifications/notifications-type.enum';
import { NotificationViewService } from '../services/notification.service';
import { NotificationDetailViewService } from '../services/notification-detail.service';
import { NotificationDetailComponent } from './notification-detail.component';
import { AbstractNotificationComponent } from './notification.abstract.component';

@Component({
  selector: 'lib-notification',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    NotificationDetailComponent,
    CommercialUiModule,
    NgxValidateCoreModule,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbDropdownModule,

    PageModule,
  ],
  providers: [
    ListService,
    NotificationViewService,
    NotificationDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
  ],
  templateUrl: './notification.component.html',
  styles: [],
})
export class NotificationComponent extends AbstractNotificationComponent {}
