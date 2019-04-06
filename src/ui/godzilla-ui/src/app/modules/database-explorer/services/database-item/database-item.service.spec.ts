import { TestBed } from '@angular/core/testing';

import { DatabaseItemService } from './database-item.service';

describe('DatabaseItemService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DatabaseItemService = TestBed.get(DatabaseItemService);
    expect(service).toBeTruthy();
  });
});
