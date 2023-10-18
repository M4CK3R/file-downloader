import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { RequestModel } from '../interfaces/request-model';

@Injectable({
  providedIn: 'root'
})
export class DownloadTaskService {
  private SSEUrl: string = `${this.baseUrl}api/sse`;

  constructor(@Inject('BASE_URL') private baseUrl: string) { }

  createEventSource(): Observable<RequestModel> {
    const eventSource = new EventSource(this.SSEUrl);
    return new Observable(observer => {
      eventSource.addEventListener('DownloadTask', (event) => {
        observer.next(JSON.parse(event.data));
      })
    })
  }
}
