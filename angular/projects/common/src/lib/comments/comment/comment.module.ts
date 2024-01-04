import { NgModule } from '@angular/core';
import { CommentComponent } from './components/comment.component';
import { CommentRoutingModule } from './comment-routing.module';

@NgModule({
  declarations: [],
  imports: [CommentComponent, CommentRoutingModule],
})
export class CommentModule {}

export function loadCommentModuleAsChild() {
  return Promise.resolve(CommentModule);
}
