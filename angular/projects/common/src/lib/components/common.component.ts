import { Component, OnInit } from '@angular/core';
import { CommonService } from '../services/common.service';

@Component({
  selector: 'lib-common',
  template: ` <p>common works!</p> `,
  styles: [],
})
export class CommonComponent implements OnInit {
  constructor(private service: CommonService) {}

  ngOnInit(): void {
    this.service.sample().subscribe(console.log);
  }
}
