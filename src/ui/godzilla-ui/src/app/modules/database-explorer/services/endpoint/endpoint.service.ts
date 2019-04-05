import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { DeviceStorageService } from 'src/app/modules/common/services/storage/device-storage.service';

@Injectable({
  providedIn: 'root'
})
export class EndpointService {

  private endpointSubject = new Subject<string>();

  private readonly endpointStorageKey = '__settings_endpoint';
  private readonly defaultEndpoint = '';

  constructor(private deviceStorage: DeviceStorageService) { }

  endpointChanged() {
    return this.endpointSubject.asObservable();
  }

  getEndpoint() {
    const storedEndpoint = this.getEndpointFromStorage();
    return storedEndpoint ? storedEndpoint : this.defaultEndpoint;
  }

  setEndpoint(endpoint: string) {
    this.setEndpointIntoStorage(endpoint);
    this.endpointSubject.next(endpoint);
  }

  private getEndpointFromStorage() {
    return this.deviceStorage.get<string>(this.endpointStorageKey);
  }

  private setEndpointIntoStorage(endpoint: string) {
    this.deviceStorage.set(this.endpointStorageKey, endpoint);
  }
}
