import { Component, OnInit } from '@angular/core';
import { AccountserviceService } from '../services/accountservice.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit{

  loginModel : any = {}

  constructor(public accountService : AccountserviceService) {}

  ngOnInit(): void {
      
  }

  login() {
    this.accountService.login(this.loginModel).subscribe({
      next : (resp) => {
        console.log(resp);
      },

      error : (err) => {
        console.log(err);
        
      }
    })
  }

  logout(){
    this.accountService.logout();
  }

}
