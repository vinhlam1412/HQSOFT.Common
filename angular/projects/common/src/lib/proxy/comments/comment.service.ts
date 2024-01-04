import type {
  CommentCreateDto,
  CommentDto,
  CommentExcelDownloadDto,
  CommentUpdateDto,
  GetCommentsInput,
} from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DownloadTokenResultDto } from '../shared/models';

@Injectable({
  providedIn: 'root',
})
export class CommentService {
  apiName = 'Common';

  create = (input: CommentCreateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CommentDto>(
      {
        method: 'POST',
        url: '/api/common/comments',
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>(
      {
        method: 'DELETE',
        url: `/api/common/comments/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CommentDto>(
      {
        method: 'GET',
        url: `/api/common/comments/${id}`,
      },
      { apiName: this.apiName, ...config }
    );

  getDownloadToken = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, DownloadTokenResultDto>(
      {
        method: 'GET',
        url: '/api/common/comments/download-token',
      },
      { apiName: this.apiName, ...config }
    );

  getList = (input: GetCommentsInput, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CommentDto>>(
      {
        method: 'GET',
        url: '/api/common/comments',
        params: {
          filterText: input.filterText,
          fromUserId: input.fromUserId,
          content: input.content,
          docId: input.docId,
          url: input.url,
          sorting: input.sorting,
          skipCount: input.skipCount,
          maxResultCount: input.maxResultCount,
        },
      },
      { apiName: this.apiName, ...config }
    );

  getListAsExcelFile = (input: CommentExcelDownloadDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, Blob>(
      {
        method: 'GET',
        responseType: 'blob',
        url: '/api/common/comments/as-excel-file',
        params: {
          downloadToken: input.downloadToken,
          filterText: input.filterText,
          name: input.name,
        },
      },
      { apiName: this.apiName, ...config }
    );

  update = (id: string, input: CommentUpdateDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CommentDto>(
      {
        method: 'PUT',
        url: `/api/common/comments/${id}`,
        body: input,
      },
      { apiName: this.apiName, ...config }
    );

  constructor(private restService: RestService) {}
}
