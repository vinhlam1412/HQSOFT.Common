import type { AuditedEntityDto, PagedAndSortedResultRequestDto } from '@abp/ng.core';

export interface CommentCreateDto {
  fromUserId?: string;
  content?: string;
  docId?: string;
  url?: string;
}

export interface CommentDto extends AuditedEntityDto<string> {
  fromUserId?: string;
  content?: string;
  docId?: string;
  url?: string;
  concurrencyStamp?: string;
}

export interface CommentExcelDownloadDto {
  downloadToken?: string;
  filterText?: string;
  name?: string;
}

export interface CommentUpdateDto {
  fromUserId?: string;
  content?: string;
  docId?: string;
  url?: string;
  concurrencyStamp?: string;
}

export interface GetCommentsInput extends PagedAndSortedResultRequestDto {
  filterText?: string;
  fromUserId?: string;
  content?: string;
  docId?: string;
  url?: string;
}
