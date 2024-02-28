import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { authGuard } from './gaurds/auth.guard';
import { NotFoundComponent } from './error-pages/not-found/not-found.component';
import { ServerErrorComponent } from './error-pages/server-error/server-error.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './gaurds/prevent-unsaved-changes.guard';
import { memberDetailedResolver } from './resolvers/member-detailed.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { adminGuard } from './gaurds/admin.guard';

const routes: Routes = [
  {path : '', component : HomeComponent},
  {path : '',
   runGuardsAndResolvers : 'always',
   canActivate : [authGuard],
   children : [
    {path : 'members', component : MemberListComponent},
    {path : 'members/:username', component : MemberDetailsComponent, resolve : {member : memberDetailedResolver}},
    {path : 'member/edit', component : MemberEditComponent, canDeactivate : [preventUnsavedChangesGuard]},
    {path : 'messages', component : MessagesComponent},
    {path : 'lists', component : ListsComponent},
    {path : 'not-found', component : NotFoundComponent},
    {path : 'server-error', component : ServerErrorComponent},
    {path : 'admin', component : AdminPanelComponent, canActivate : [adminGuard]}
   ]
  },
  {path : '**', component : HomeComponent, pathMatch : 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
