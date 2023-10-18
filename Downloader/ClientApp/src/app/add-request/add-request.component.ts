import { Component } from '@angular/core';
import { AddRequestModel } from '../interfaces/add-request-model';
import { RequestService } from '../services/request.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-request',
  templateUrl: './add-request.component.html',
  styleUrls: ['./add-request.component.scss']
})
export class AddRequestComponent {
  public data: AddRequestModel = {
    Name: '',
    Url: '',
    Retry: true,
    MaxTries: 0,
  };

  constructor(private requestService: RequestService, private router: Router) { }

  onSubmit() {
    this.requestService.add(this.data).subscribe(() => {
      console.log('Request added successfully!');
      this.router.navigate(['/']);
    });
  }
}
