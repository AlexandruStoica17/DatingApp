import { Component, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
// export class HomeComponent implements OnInit {
  export class HomeComponent {
    // http = inject(HttpClient);
  registerMode = false;
    // users: any;

    // ngOnInit(): void {
    //   this.getUsers();
    // }

  registerToggle(){
    this.registerMode = !this.registerMode
  }

cancelRegisterMode(event: boolean){
  this.registerMode = event;
}

//     getUsers(){
//     this.http.get('https://localhost:5001/api/users').subscribe({
//       //next: () => {}, ex de function ce nu face nimic
//       next: response => this.users = response,
//       error: error => console.log(error),
//       complete: () => console.log('Request completed') //cand e completed e automat unsubscribed
//     })
// }
}
