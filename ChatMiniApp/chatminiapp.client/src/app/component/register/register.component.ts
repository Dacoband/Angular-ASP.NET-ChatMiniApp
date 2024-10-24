import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterModel } from '../../models/register.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerModel: RegisterModel = new RegisterModel();

  constructor(private http: HttpClient,
              private router: Router)
  {
  }

  setImage(event: any) {
    this.registerModel.file = event.target.files[0];
  }

  register() {
    const formData = new FormData();
    formData.append('name', this.registerModel.name);
    formData.append('file', this.registerModel.file, this.registerModel.file.name);

    this.http.post("https://localhost:7222/api/AuthContronller/Register/login", formData).subscribe(res => {
      this.router.navigateByUrl("/");
    });
  }
}
