import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { MessagesComponent } from './components/messages/messages.component';
import { ListComponent } from './components/list/list.component';
import { DetailComponent } from './components/members/detail/detail.component';
import { MemberListComponent } from './components/members/member-list/member-list.component';
import { authGuard } from './_guards/auth.guard';
import { TestErrorComponent } from './components/errors/test-error/test-error.component';
import { NotFoundComponent } from './components/errors/not-found/not-found.component';
import { ServerErrorComponent } from './components/errors/server-error/server-error.component';
import { MemberEditComponent } from './components/members/member-edit/member-edit.component';
import { preventUnsaveChangesGuard } from './_guards/prevent-unsave-changes.guard';

const routes: Routes = [
  { path: "", component: HomeComponent },
  { path: "",
    runGuardsAndResolvers: "always",
    canActivate: [authGuard],
    children: [
      { path: "members", component: MemberListComponent},
      { path: "members/:username", component: DetailComponent },
      { path: "member/edit", component: MemberEditComponent, canDeactivate: [preventUnsaveChangesGuard] },
      { path: "list", component: ListComponent },
      { path: "messages", component: MessagesComponent }      
    ]
  },
  { path: "errors", component: TestErrorComponent },
  { path: "not-found", component: NotFoundComponent },
  { path: "server-error", component: ServerErrorComponent },
  { path: "**", component: NotFoundComponent, pathMatch: "full" }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
