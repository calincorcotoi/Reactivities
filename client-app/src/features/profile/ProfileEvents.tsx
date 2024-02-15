import { Tab, TabProps } from "semantic-ui-react";
import Events from "./Events";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";
import { useEffect } from "react";

export default observer(function ProfileEvents() {
  const {
    profileStore: { loadEvents },
  } = useStore();
  useEffect(() => {
    loadEvents("past");
  });
  const handleTabChange = (_: React.MouseEvent<HTMLDivElement, MouseEvent>, d: TabProps) => {
    switch (d.activeIndex) {
      case 0:
        loadEvents("past");
        break;
      case 1:
        loadEvents("future");
        break;
      case 2:
        loadEvents("hosting");
        break;

      default:
        break;
    }
  };

  const panes = [
    {
      menuItem: "Future Events",
      render: () => <Events />,
    },
    {
      menuItem: "Past Events",
      render: () => <Events />,
    },
    { menuItem: "Hosting", render: () => <Events /> },
  ];

  return <Tab panes={panes} onTabChange={handleTabChange} />;
});
