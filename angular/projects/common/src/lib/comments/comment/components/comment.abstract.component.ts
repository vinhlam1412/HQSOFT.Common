import { ListService, TrackByService } from '@abp/ng.core';
import { Component, OnInit, inject } from '@angular/core';

import type { CommentDto } from '../../../proxy/comments/models';
import { CommentViewService } from '../services/comment.service';
import { CommentDetailViewService } from '../services/comment-detail.service';

@Component({
  template: '',
})
export abstract class AbstractCommentComponent implements OnInit {
  public readonly list = inject(ListService);
  public readonly track = inject(TrackByService);
  public readonly service = inject(CommentViewService);
  public readonly serviceDetail = inject(CommentDetailViewService);
  protected title = 'Common::Comments';

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

  update(record: CommentDto) {
    this.serviceDetail.update(record);
  }

  delete(record: CommentDto) {
    this.service.delete(record);
  }

  exportToExcel() {
    this.service.exportToExcel();
  }
}
