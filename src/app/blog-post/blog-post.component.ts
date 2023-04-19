import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { environment } from '../../environments/environment';



@Component({
  selector: 'app-blog-post',
  templateUrl: './blog-post.component.html',
  styleUrls: ['./blog-post.component.scss']
})
export class BlogPostComponent implements OnInit {
  postId: string | undefined;

  //constructor(private http: HttpClient) {}
  constructor(private http: HttpClient, private route: ActivatedRoute) {}


  ngOnInit() {
    this.route.params.subscribe(params => {
      this.postId = params['postId'];
      this.fetchData();
    });
  }

  fetchData() {
    //const postId = '123';
    // Get the postId from the route
    //const postId = this.route.snapshot.paramMap.get('postId');

     // Get the postId from the route and provide a fallback value
    const postId = this.route.snapshot.paramMap.get('postId') ?? '';

  // Use the BFF URL from the environment configuration
    const bffUrl = environment.bffUrl;

    const url = `${bffUrl}/post/${encodeURIComponent(postId)}`;


    //const url = `https://localhost:7290/Proxy/post/${encodeURIComponent(postId)}`;

    this.http.get(url).subscribe((response) => {
      // Process the response
      // Update the Open Graph tags using Angular Meta service
    });
  }
}
