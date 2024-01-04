import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CommonComponent } from './components/common.component';
import { loadCommentModuleAsChild } from './comments/comment/comment.module';
import { loadNotificationModuleAsChild } from './notifications/notification/notification.module';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: CommonComponent,
  },
  { path: 'comments', loadChildren: loadCommentModuleAsChild },
  { path: 'notifications', loadChildren: loadNotificationModuleAsChild },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CommonRoutingModule {}
