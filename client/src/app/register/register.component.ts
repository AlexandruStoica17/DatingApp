import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent { //RegisterComponent is the child component
private accountService = inject(AccountService);
private toastr = inject(ToastrService);
// usersFromHomeComponent = input.required<any>() comentat pt vid 57 pt who s your fav user
cancelRegister = output<boolean>();
model: any={}

register(){
  this.accountService.register(this.model).subscribe({
    next: response => {
      console.log(response);
      this.cancel();
    },
    error: error => this.toastr.error(error.error)
    
  })
}

cancel(){
  this.cancelRegister.emit(false); //emitter
}
}
