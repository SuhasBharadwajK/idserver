import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {

  constructor() { }

  get authApiURI() {
    return 'https://localhost:44350/api';
  }

  get resourceApiURI() {
    return 'https://localhost:44350/api';
  }
}
