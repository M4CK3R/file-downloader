import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { RequestModel } from '../interfaces/request-model';
import { environment } from 'src/environments/environment';
import { AddRequestModel } from '../interfaces/add-request-model';
import { ToastService } from './toast.service';
import { ToastType } from '../interfaces/toast';


@Injectable({
  providedIn: 'root'
})
export class RequestService {
  private getAllUrl: string = 'api/Request/GetAll';
  private addUrl: string = 'api/Request/Add';
  constructor(private toastService: ToastService, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {

  }

  private toRequest(data: any): RequestModel {
    return {
      id: data.id,
      url: data.url,
      name: data.name,
      isDone: data.isDone,
      progress: data.progress ?? null
    };
  }

  getAll(): Observable<RequestModel[]> {
    return this.http.get<RequestModel[]>(this.getAllUrl).pipe(
      map((data: any[]) => data.map((item: any) => this.toRequest(item))),
      catchError((e) => this.handleError(e, this.toastService))
    );
  }

  add(addRequestModel: AddRequestModel): Observable<RequestModel> {
    return this.http.post(this.addUrl, addRequestModel).pipe(
      map((data: any) => this.toRequest(data)),
      catchError((e) => this.handleError(e, this.toastService))
    );
  }

  delete(id: string): Observable<RequestModel> {
    return this.http.delete(`${this.baseUrl}api/Request/Delete?id=${id}`).pipe(
      map((data: any) => this.toRequest(data)),
      catchError((e) => this.handleError(e, this.toastService))
    );
  }

  handleError(error: HttpErrorResponse, toastService: ToastService) { 
    toastService.addToast({
      type: ToastType.Error,
      message: error.error,
      title: 'Error'
    });
    return throwError(() => new Error(error.error));
   }
}