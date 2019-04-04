import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EndpointService {

  private endpointSubject = new Subject<string>();

  private readonly defaultEndpoint = '';

  constructor() { }

  endpointChanged() {
    return this.endpointSubject.asObservable();
  }

  getEndpoint() {
    return this.defaultEndpoint;
  }
}
