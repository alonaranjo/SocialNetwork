import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MessagesComponent } from './messages/messages.component';
import { ListComponent } from './list/list.component';
import { DetailComponent } from './members/detail/detail.component';
import { MemberListComponent } from './members/member-list/member-list.component';

const routes: Routes = [
  { path: "", component: HomeComponent },
  { path: "members", component: MemberListComponent },
  { path: "members/:id", component: DetailComponent },
  { path: "list", component: ListComponent },
  { path: "messages", component: MessagesComponent },
  { path: "**", component: HomeComponent, pathMatch: "full" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
