import { Component, OnInit } from '@angular/core';
import { AccountserviceService } from '../services/accountservice.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{

  loginModel : any = {}

  constructor(public accountService : AccountserviceService, private router : Router, private toaster : ToastrService) {}

  ngOnInit(): void {
      
  }

  login() {
    this.accountService.login(this.loginModel).subscribe({
      next : (resp) => {
        this.router.navigateByUrl('/members');
        this.toaster.success('Login Successfull');
      },

      error : (err) => {
        console.log(err);
        this.toaster.error(err.error);
      }
      
    })
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}
