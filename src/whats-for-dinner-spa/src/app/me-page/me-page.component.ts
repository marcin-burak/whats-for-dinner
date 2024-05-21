import { Component, Inject, OnInit } from '@angular/core';
import { ApiClient, GetMeResponse } from '../../api';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-me-page',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './me-page.component.html',
  styleUrl: './me-page.component.scss'
})
export class MePageComponent implements OnInit {

  private readonly api: ApiClient;

  private _me?: GetMeResponse;
  public get me() {
    return this._me
  };

  constructor(@Inject(ApiClient) api: ApiClient) {
    this.api = api;
  }

  ngOnInit(): void {
    this.api.getMe().subscribe(response => this._me = response);
  }
}
