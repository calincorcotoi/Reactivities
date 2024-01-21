import { User } from "./user";

export interface IProfile {
  username: string;
  displayName: string;
  image?: string;
  bio?: string;
  photos?: Photo[];
}

export class Profile implements IProfile {
  username: string;
  displayName: string;
  image?: string;
  bio?: string;
  photos?: Photo[];
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
