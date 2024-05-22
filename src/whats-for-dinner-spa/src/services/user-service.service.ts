import { Injectable } from '@angular/core';
import { ApiClient, GetMeResponse } from '../api';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UserService {

  constructor(private readonly api: ApiClient) {}

  public get currentUser(): Observable<GetMeResponse> {
    return this.api.getMe();
  }
}
