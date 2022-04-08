import React, { useState } from 'react';
import {
  Text,
  StyleSheet,
  TouchableOpacity,
  Modal,
  FlatList,
  View,
} from 'react-native';
import { colors, safeAreaSize } from '../../theme/default';
import { size } from '../../utils/dimensions';
import Label from '../Label';
import Icon from 'react-native-vector-icons/FontAwesome5';
import CloseButton from '../CloseButton';
import Divider from '../Divider';

interface IDropdownProps<T> {
  label?: string;
  data: T[];
  value?: string;
  optionExtractor: (item: T) => string;
  placeHolder?: string;
}

const styles = StyleSheet.create({
  container: {
    backgroundColor: colors.background.secondary,
    borderRadius: size(3.4),
    paddingHorizontal: safeAreaSize,
    paddingVertical: size(3),
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  text: {
    fontWeight: '500',
    fontSize: size(4),
  },
  modalHeaderContainer: {
    padding: safeAreaSize,
  },
});

const Selector = <T extends unknown>({
  label,
  placeHolder,
  value,
  data,
  optionExtractor,
}: IDropdownProps<T>) => {
  const [modalIsVisible, setModalIsVisible] = useState(false);

  const closeModal = () => {
    setModalIsVisible(false);
  };

  const ModalHeader = () => {
    return (
      <View style={styles.modalHeaderContainer}>
        <CloseButton onPress={closeModal} />
      </View>
    );
  };

  const renderItem = ({ item }: any) => {
    return (
      <View
        style={{
          padding: safeAreaSize,
        }}>
        <Text>{optionExtractor(item)}</Text>
      </View>
    );
  };

  return (
    <>
      {label && <Label>{label}</Label>}
      <TouchableOpacity
        onPress={() => setModalIsVisible(true)}
        style={styles.container}>
        <Text style={[styles.text, { color: value ? 'black' : '#636366' }]}>
          {value || placeHolder || 'Selecione'}
        </Text>
        <Icon
          size={size(4.5)}
          name="caret-down"
          color={value ? 'black' : '#636366'}
        />
      </TouchableOpacity>
      <Modal
        visible={modalIsVisible}
        animationType="slide"
        onRequestClose={closeModal}>
        <FlatList
          data={data}
          renderItem={renderItem}
          ListHeaderComponent={ModalHeader}
          keyExtractor={(_, index) => String(index)}
          ItemSeparatorComponent={() => <Divider />}
        />
      </Modal>
    </>
  );
};

export default Selector;
