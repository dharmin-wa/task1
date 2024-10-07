import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserListComponent } from './pages/user-list/user-list.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { UserDetailComponent } from './pages/user-detail/user-detail.component';

const routes: Routes = [
  // { path: 'add-user', component: UserFormComponent },
  { path: 'users', component: UserListComponent },
  { 'path': '', 'redirectTo': '/login', 'pathMatch': 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'home', component: UserDetailComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
