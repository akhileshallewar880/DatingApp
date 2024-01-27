import { HttpClient } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{

  allUsers : any ;

  registerMode : boolean = false;

  constructor(private http : HttpClient) {
    
  }

  ngOnInit(): void {
   
  }

register(event : boolean) {
  this.registerMode = event;
}

}
