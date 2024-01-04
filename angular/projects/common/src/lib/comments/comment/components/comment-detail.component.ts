import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CommercialUiModule } from '@volo/abp.commercial.ng.ui';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbDateAdapter, NgbDatepickerModule, NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { DateAdapter } from '@abp/ng.theme.shared/extensions';
import { CommentDetailViewService } from '../services/comment-detail.service';

@Component({
  selector: 'lib-comment-detail',
  changeDetection: ChangeDetectionStrategy.Default,
  standalone: true,
  imports: [
    CoreModule,
    ThemeSharedModule,
    CommercialUiModule,
    ReactiveFormsModule,
    NgbDatepickerModule,
    NgbNavModule,
  ],
  providers: [{ provide: NgbDateAdapter, useClass: DateAdapter }],
  templateUrl: './comment-detail.component.html',
  styles: [],
})
export class CommentDetailComponent {
  public readonly service = inject(CommentDetailViewService);
}
