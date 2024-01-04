import { Injectable, inject } from '@angular/core';
import { ConfirmationService, Confirmation } from '@abp/ng.theme.shared';
import { ABP, downloadBlob, ListService, PagedResultDto } from '@abp/ng.core';
import { filter, switchMap, finalize } from 'rxjs/operators';
import type { GetCommentsInput, CommentDto } from '../../../proxy/comments/models';
import { CommentService } from '../../../proxy/comments/comment.service';

export abstract class AbstractCommentViewService {
  protected readonly proxyService = inject(CommentService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly list = inject(ListService);

  isExportToExcelBusy = false;

  data: PagedResultDto<CommentDto> = {
    items: [],
    totalCount: 0,
  };

  filters = {} as GetCommentsInput;

  delete(record: CommentDto) {
    this.confirmationService
      .warn('Common::DeleteConfirmationMessage', 'Common::AreYouSure', {
        messageLocalizationParams: [],
      })
      .pipe(
        filter(status => status === Confirmation.Status.confirm),
        switchMap(() => this.proxyService.delete(record.id))
      )
      .subscribe(this.list.get);
  }

  hookToQuery() {
    const getData = (query: ABP.PageQueryParams) =>
      this.proxyService.getList({
        ...query,
        ...this.filters,
        filterText: query.filter,
      });

    const setData = (list: PagedResultDto<CommentDto>) => (this.data = list);

    this.list.hookToQuery(getData).subscribe(setData);
  }

  clearFilters() {
    this.filters = {} as GetCommentsInput;
    this.list.get();
  }

  exportToExcel() {
    this.isExportToExcelBusy = true;
    this.proxyService
      .getDownloadToken()
      .pipe(
        switchMap(({ token }) =>
          this.proxyService.getListAsExcelFile({
            downloadToken: token,
            filterText: this.list.filter,
          })
        ),
        finalize(() => (this.isExportToExcelBusy = false))
      )
      .subscribe(result => {
        downloadBlob(result, 'Comment.xlsx');
      });
  }
}
