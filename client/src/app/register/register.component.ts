import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountserviceService } from '../services/accountservice.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
  
  @Output() cancelBtn = new EventEmitter();

  model: any = {}

  constructor(private accountService : AccountserviceService) {
    
  }

  ngOnInit(): void {
      
  }

  register() {
    this.accountService.register(this.model).subscribe({
      next: () => {
        this.cancel();
      },
      error: (err) => console.log(err)
      
    })
  }

  cancel() {
    this.cancelBtn.emit(false);
  }

}
