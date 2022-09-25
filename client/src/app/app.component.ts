import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating App';
  Users: any;
  constructor(private http:HttpClient){

  }

  ngOnInit() {
    
    this.getUsers;
  }
  // getUsers(){
  //   this.http.get('https://localhost:5004/apiii/users').subscribe(Response=>{
  //     this.Users = Response;
  //   }, error=>{
  //     console.log(error);
  //   })
  // }
  getUsers() {
    this.http.get('https://localhost:5004/apiii/users').subscribe({
      next: response => this.Users = response,
      error: error => console.log(error)
    })
  }
}
