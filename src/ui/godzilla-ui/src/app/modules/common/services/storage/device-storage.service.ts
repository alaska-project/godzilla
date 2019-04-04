import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DeviceStorageService {

  constructor() { }

  get<T>(key: string): T {
    return JSON.parse(localStorage.getItem(key));
  }

  set(key: string, value: any) {
    localStorage.setItem(key, JSON.stringify(value));
  }
}
