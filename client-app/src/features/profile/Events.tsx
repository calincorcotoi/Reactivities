import { Card, CardGroup, TabPane } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import { format } from "date-fns";

export default observer(function Events() {
  const { profileStore } = useStore();

  return (
    <TabPane loading={profileStore.loadingEvents}>
      <CardGroup itemsPerRow={4}>
        {profileStore.events.map((e) => (
          <Card
            key={e.id}
            image={`/assets/categoryImages/${e.category}.jpg`}
            header={e.title}
            meta={format(e.date, "dd MMM yyyy")}
          />
        ))}
      </CardGroup>
    </TabPane>
  );
});
