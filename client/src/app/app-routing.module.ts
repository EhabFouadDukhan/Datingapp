import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MembersDetailComponent } from './members/members-detail/members-detail.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path: '',component:HomeComponent},
  {path: 'members',component:MembersListComponent,canActivate:[AuthGuard]},
  {path: 'members/:id',component:MembersDetailComponent,canActivate:[AuthGuard]},
  {path: 'lists',component:ListsComponent,canActivate:[AuthGuard]},
  {path: 'messages',component:MessagesComponent,canActivate:[AuthGuard]},
  {path: '**',component:HomeComponent,pathMatch:'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
