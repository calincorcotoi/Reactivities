import { Photo, Profile, ProfileFormValues } from "../models/profile";
import { makeAutoObservable, reaction, runInAction } from "mobx";
import agent from "../api/agent";
import { toast } from "react-toastify";
import { store } from "./store";
import { Event } from "../models/event";

export default class ProfileStore {
  profile: Profile | null = null;
  loadingProfile = false;
  uploading = false;
  loading = false;
  followings: Profile[] = [];
  loadingFollowings = false;
  activeTab: number = 0;
  events: Event[] = [];
  loadingEvents = false;
  constructor() {
    makeAutoObservable(this);

    reaction(
      () => this.activeTab,
      (activeTab) => {
        if (activeTab === 3 || activeTab === 4) {
          const predicate = activeTab === 3 ? "followers" : "following";
          this.loadFollowings(predicate);
        } else {
          this.followings = [];
        }
      }
    );
  }

  get isCurrentUser() {
    if (store.userStore.user && this.profile) {
      return store.userStore.user.username === this.profile.username;
    }
    return false;
  }

  setActiveTab = (activeTab: any) => {
    this.activeTab = activeTab;
  };

  loadProfile = async (username: string) => {
    this.loadingProfile = true;
    try {
      const profile = await agent.Profiles.get(username);
      runInAction(() => {
        this.profile = profile;
        this.loadingProfile = false;
      });
    } catch (error) {
      toast.error("Problem loading profile");
      runInAction(() => {
        this.loadingProfile = false;
      });
    }
  };

  updateProfile = async (newProfile: ProfileFormValues) => {
    try {
      await agent.Profiles.update(newProfile);
      runInAction(() => {
        this.profile!.displayName = newProfile.displayName;
        store.userStore.user!.displayName = newProfile.displayName;
        this.profile!.bio = newProfile.bio;
      });
    } catch (error) {
      toast.error("Problem updating profile");
    }
  };

  uploadPhoto = async (file: any) => {
    this.uploading = true;
    try {
      const response = await agent.Profiles.uploadPhoto(file);
      const photo = response.data;
      runInAction(() => {
        if (this.profile) {
          this.profile.photos?.push(photo);
          if (photo.isMain && store.userStore.user) {
            store.userStore.setImage(photo.url);
            this.profile.image = photo.url;
          }
        }
        this.uploading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => (this.uploading = false));
    }
  };

  setMainPhoto = async (photo: Photo) => {
    this.loading = true;
    try {
      await agent.Profiles.setMainPhoto(photo.id);
      store.userStore.setImage(photo.url);
      runInAction(() => {
        if (this.profile && this.profile.photos) {
          this.profile.photos.find((a) => a.isMain)!.isMain = false;
          this.profile.photos.find((a) => a.id === photo.id)!.isMain = true;
          this.profile.image = photo.url;
          this.loading = false;
        }
      });
    } catch (error) {
      console.log(error);
      runInAction(() => (this.loading = false));
    }
  };

  deletePhoto = async (photo: Photo) => {
    this.loading = true;
    try {
      await agent.Profiles.deletePhoto(photo.id);
      runInAction(() => {
        if (this.profile) {
          this.profile.photos = this.profile.photos?.filter((a) => a.id !== photo.id);
          this.loading = false;
        }
      });
    } catch (error) {
      toast.error("Problem deleting photo");
      this.loading = false;
    }
  };

  updateFollowing = async (username: string, following: boolean) => {
    this.loading = true;
    try {
      await agent.Profiles.updateFollowing(username);
      store.activityStore.updateAttendeeFollowing(username);
      runInAction(() => {
        if (
          this.profile &&
          this.profile.username !== store.userStore.user?.username &&
          this.profile.username === username
        ) {
          following ? this.profile.followersCount++ : this.profile.followersCount--;
          this.profile.following = !this.profile.following;
        }
        if (this.profile && this.profile.username === store.userStore.user?.username) {
          following ? this.profile.followingCount++ : this.profile.followingCount--;
        }
        this.followings.forEach((profile) => {
          if (profile.username === username) {
            profile.following ? profile.followersCount-- : profile.followersCount++;
            profile.following = !profile.following;
          }
        });
        this.loading = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => (this.loading = false));
    }
  };

  loadFollowings = async (predicate: string) => {
    this.loadingFollowings = true;
    try {
      const followings = await agent.Profiles.listFollowings(this.profile!.username, predicate);
      runInAction(() => {
        this.followings = followings;
        this.loadingFollowings = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => (this.loadingFollowings = false));
    }
  };

  loadEvents = async (predicate: string) => {
    this.loadingEvents = true;
    try {
      const events = await agent.Profiles.listEvents(this.profile!.username, predicate);
      events.forEach((e) => (e.date = new Date(e.date)));
      runInAction(() => {
        this.events = events;
        this.loadingEvents = false;
      });
    } catch (error) {
      console.log(error);
      runInAction(() => (this.loadingEvents = false));
    }
  };
}
