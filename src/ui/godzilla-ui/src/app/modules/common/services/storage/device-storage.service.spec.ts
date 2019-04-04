import { TestBed } from '@angular/core/testing';

import { DeviceStorageService } from './device-storage.service';

describe('DeviceStorageService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DeviceStorageService = TestBed.get(DeviceStorageService);
    expect(service).toBeTruthy();
  });
});
