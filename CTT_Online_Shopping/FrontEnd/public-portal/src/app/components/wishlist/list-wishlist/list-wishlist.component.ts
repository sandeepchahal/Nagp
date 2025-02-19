import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { WishListService } from '../../../services/wishlist.service';
import { WishListQuery } from '../../../models/wishlist.model';
import { LoaderComponent } from '../../common/loader/loader.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-wishlist',
  standalone: true,
  imports: [CommonModule, LoaderComponent],
  templateUrl: './list-wishlist.component.html',
  styleUrl: './list-wishlist.component.css',
})
export class ListWishlistComponent implements OnInit {
  wishlist: WishListQuery[] = [];
  showLoading: boolean = true;
  constructor(
    private wishlistService: WishListService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.wishlistService.wishlist$.subscribe((wishlist) => {
      this.wishlist = wishlist;
    });
    this.wishlistService.GetWishlists(); // Fetch wishlist initially
    this.showLoading = false;
  }

  removeFromWishlist(id: string) {
    this.wishlistService.RemoveWishlist(id);
    this.showLoading = false;
  }
  goToDetail(id: string) {
    this.router.navigate(['/product/item', id]);
  }
}
