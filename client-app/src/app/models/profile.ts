import { User } from "./user";

export interface IProfile {
  username: string;
  displayName: string;
  image?: string;
  bio?: string;
  photos?: Photo[];
  followersCount: number;
  followingCount: number;
  following: boolean;
}

export class Profile implements IProfile {
  username: string;
  displayName: string;
  image?: string;
  bio?: string;
  photos?: Photo[];
  followersCount: number = 0;
  followingCount: number = 0;
  following: boolean = false;

  constructor(user: User) {
    this.username = user.username;
    this.image = user.image;
    this.displayName = user.displayName;
  }
}

export interface Photo {
  id: string;
  url: string;
  isMain: boolean;
}

export interface ProfileFormValues {
  displayName: string;
  bio?: string;
}
