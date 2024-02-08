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

const routes: Routes = [
  {path : '', component : HomeComponent},
  {path : '',
   runGuardsAndResolvers : 'always',
   canActivate : [authGuard],
   children : [
    {path : 'members', component : MemberListComponent},
    {path : 'members/:username', component : MemberDetailsComponent},
    {path : 'messages', component : MessagesComponent},
    {path : 'lists', component : ListsComponent},
    {path : 'not-found', component : NotFoundComponent},
    {path : 'server-error', component : ServerErrorComponent}
   ]
  },
  {path : '**', component : HomeComponent, pathMatch : 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
