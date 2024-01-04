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

import { CommentViewService } from '../services/comment.service';
import { CommentDetailViewService } from '../services/comment-detail.service';
import { CommentDetailComponent } from './comment-detail.component';
import { AbstractCommentComponent } from './comment.abstract.component';

@Component({
  selector: 'lib-comment',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommentDetailComponent,
    CommercialUiModule,
    NgxValidateCoreModule,
    NgbCollapseModule,
    NgbDatepickerModule,
    NgbDropdownModule,

    PageModule,
  ],
  providers: [
    ListService,
    CommentViewService,
    CommentDetailViewService,
    { provide: NgbDateAdapter, useClass: DateAdapter },
  ],
  templateUrl: './comment.component.html',
  styles: [],
})
export class CommentComponent extends AbstractCommentComponent {}
