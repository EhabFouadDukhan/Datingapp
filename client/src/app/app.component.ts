import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating app';
  users: any;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getUsers();
  }

  getUsers() {
    this.http.get("https://localhost:5004/apiii/users").subscribe( {
      next: response => this.users = response,
      error: error => console.log(error)
    })
  }
}

// getUsers() {
  //   this.http.get('https://localhost:5004/apiii/users').subscribe({
  //     next: response => this.Users = response,
  //     error: error => console.log(error)
  //   })
  
