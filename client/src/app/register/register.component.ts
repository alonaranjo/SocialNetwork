import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
  @Output() cancelRegister = new EventEmitter();
  model: any = { }
  constructor(private accountServices: AccountService, private toast: ToastrService){ } 

  ngOnInit(): void {
   
  }

  register(){
    this.accountServices.register(this.model).subscribe({
      next: () => this.cancel()
    });
  }

  cancel(){
    this.cancelRegister.emit(false);
  }
}
