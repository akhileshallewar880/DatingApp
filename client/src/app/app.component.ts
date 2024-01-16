import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  users : any;

  constructor(private http: HttpClient) {

  }

  ngOnInit(): void {
    this.http.get('http://localhost:5054/api/Users').subscribe({
      next : resp => this.users = resp,
      error : err => console.log(err),
      complete: () => console.log('Request completed')
  })
  }
}
