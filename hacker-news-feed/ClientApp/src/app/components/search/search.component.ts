import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent {
  id:number;

  constructor(private router: Router) {
  }

  onSearch() {
    if(!this.id || isNaN(this.id)){
      alert("Only Id Numbers are valid");
      this.id = undefined;
      return;
    };

    this.router.navigate([`/${this.id}`]);
  }
}
